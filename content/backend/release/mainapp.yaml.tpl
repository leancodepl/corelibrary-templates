apiVersion: apps/v1
kind: Deployment
metadata:
  name: lncdapp-mainapp
  namespace: ${NAMESPACE}
  labels:
    app: lncdapp
    component: mainapp
    environment: ${ENVIRONMENT}
spec:
  selector:
    matchLabels:
      app: lncdapp
      component: mainapp
      environment: ${ENVIRONMENT}
  replicas: 1
  revisionHistoryLimit: 3
  template:
    metadata:
      labels:
        app: lncdapp
        component: mainapp
        environment: ${ENVIRONMENT}
    spec:
      containers:
        - name: mainapp
          image: dockerrepository/lncdapp-mainapp:${APP_VERSION}
          resources:
            requests:
              cpu: 50m
              memory: 200Mi
            limits:
              cpu: 500m
              memory: 300Mi
          envFrom:
            - secretRef:
                name: lncdapp-mainapp
          ports:
            - containerPort: 80
          livenessProbe:
            httpGet:
              path: /live/health
              port: 80
            initialDelaySeconds: 15
            periodSeconds: 5
            timeoutSeconds: 5
          readinessProbe:
            httpGet:
              path: /live/ready
              port: 80
            initialDelaySeconds: 20
            periodSeconds: 10
            timeoutSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: lncdapp-mainapp
  namespace: ${NAMESPACE}
  labels:
    app: lncdapp
    component: mainapp
    environment: ${ENVIRONMENT}
spec:
  type: ClusterIP
  ports:
    - port: 80
  selector:
    app: lncdapp
    component: mainapp
    environment: ${ENVIRONMENT}
