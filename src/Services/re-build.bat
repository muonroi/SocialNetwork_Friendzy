@echo off
REM Loop through each service directory and run docker-compose build

cd Account.Service
docker-compose build
cd ..

cd API.Intergration.Config.Service
docker-compose build
cd ..

cd Authenticate.Service
docker-compose build
cd ..

cd Distance.Service
docker-compose build
cd ..

cd Hangfire.Service
docker-compose build
cd ..

cd Management.Friends.Action.Service
docker-compose build
cd ..

cd Management.Photo.Service
docker-compose build
cd ..

cd Matched.Friend.Service
docker-compose build
cd ..

cd Message.Service
docker-compose build
cd ..

cd Post.Service
docker-compose build
cd ..

cd SearchPartner.Service
docker-compose build
cd ..

cd Setting.Service
docker-compose build
cd ..

cd User.Service
docker-compose build
cd ..

echo All services have been built.
pause
