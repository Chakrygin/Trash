@echo off

for /f %%a in ('docker container ls --filter name^=redis --quiet') do (
    docker container kill "%%a"
)

docker run --rm --detach ^
    --name redis ^
    --publish 6379:6379 ^
    redis
