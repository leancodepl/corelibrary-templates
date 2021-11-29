apiVersion: batch/v1
kind: Job
metadata:
  name: nameLower-migrations
  namespace: ${NAMESPACE}
  labels:
    project: nameLower
    component: migrations
spec:
  backoffLimit: 2
  template:
    metadata:
      labels:
        project: nameLower
        component: migrations
    spec:
      restartPolicy: OnFailure
      containers:
        - name: migrations
          image: dockerrepository/nameLower-migrations:${APP_VERSION}
          resources:
            requests:
              cpu: 50m
              memory: 200Mi
          env:
            - name: SqlServer__ConnectionString
              valueFrom:
                secretKeyRef:
                  name: nameLower-migrations
                  key: SqlServer__ConnectionString
