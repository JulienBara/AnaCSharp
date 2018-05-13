echo "> cd /root/repo/AnaCSharp"
cd /root/repo/AnaCSharp

echo "> git pull"
git pull

echo "> docker-compose build"
docker-compose build

echo "> docker-compose up -d"
docker-compose up -d
