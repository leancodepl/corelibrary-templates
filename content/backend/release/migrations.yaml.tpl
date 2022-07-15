apiVersion: batch/v1
kind: Job
metadata:
  name: lncdapp-migrations
  namespace: ${NAMESPACE}
  labels:
    project: lncdapp
    component: migrations
spec:
  backoffLimit: 2
  template:
    metadata:
      labels:
        project: lncdapp
        component: migrations
        aadpodidbinding: lncdapp-migrations
    spec:
      restartPolicy: OnFailure
      containers:
        - name: migrations
          image: dockerrepository/lncdapp-migrations:${APP_VERSION}
          resources:
            requests:
              cpu: 50m
              memory: 200Mi
          env:
            - name: SqlServer__ConnectionString
              valueFrom:
                secretKeyRef:
                  name: lncdapp-migrations
                  key: SqlServer__ConnectionString
