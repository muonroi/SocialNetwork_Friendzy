@echo off
REM Đăng nhập vào Docker Hub
docker login

REM Gán thẻ và push các image hiện có với tag mới

docker tag distance-service:linux-latest muonroi/distance-service:v1.0
docker push muonroi/distance-service:v1.0

docker tag search-agg-service:linux-latest muonroi/search-agg-service:v1.0
docker push muonroi/search-agg-service:v1.0

docker tag user-service:linux-latest muonroi/user-service:v1.0
docker push muonroi/user-service:v1.0

docker tag post-agg-service:linux-latest muonroi/post-agg-service:v1.0
docker push muonroi/post-agg-service:v1.0

docker tag api-intergration-config-service:linux-latest muonroi/api-intergration-config-service:v1.0
docker push muonroi/api-intergration-config-service:v1.0

docker tag matched-friend-service:linux-latest muonroi/matched-friend-service:v1.0
docker push muonroi/matched-friend-service:v1.0

docker tag post-service:linux-latest muonroi/post-service:v1.0
docker push muonroi/post-service:v1.0

docker tag setting-service:linux-latest muonroi/setting-service:v1.0
docker push muonroi/setting-service:v1.0

docker tag search-partners-service:linux-latest muonroi/search-partners-service:v1.0
docker push muonroi/search-partners-service:v1.0

docker tag management-photo-service:linux-latest muonroi/management-photo-service:v1.0
docker push muonroi/management-photo-service:v1.0

docker tag account-service:linux-latest muonroi/account-service:v1.0
docker push muonroi/account-service:v1.0

docker tag authenticate-service:linux-latest muonroi/authenticate-service:v1.0
docker push muonroi/authenticate-service:v1.0

docker tag muonroi/minio-resource-db-friendzy:latest muonroi/minio-resource-db-friendzy:v1.0
docker push muonroi/minio-resource-db-friendzy:v1.0

docker tag muonroi/postgres-db-friendzy:alpine3.20 muonroi/postgres-db-friendzy:v1.0
docker push muonroi/postgres-db-friendzy:v1.0

docker tag muonroi/portainer-friendzy:latest muonroi/portainer-friendzy:v1.0
docker push muonroi/portainer-friendzy:v1.0

docker tag muonroi/mssqldbfriendzy:2019-latest muonroi/mssqldbfriendzy:v1.0
docker push muonroi/mssqldbfriendzy:v1.0

docker tag muonroi/mysqldbfriendzy:8.0.29 muonroi/mysqldbfriendzy:v1.0
docker push muonroi/mysqldbfriendzy:v1.0