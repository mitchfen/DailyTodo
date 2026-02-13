# dotnet publish must be run before building this image, and the output should be in ./publish/wwwroot/
FROM nginx:alpine

RUN apk update \
    && apk upgrade \
    && rm -rf /var/cache/apk/*

COPY nginx.conf /etc/nginx/nginx.conf

COPY ./publish/wwwroot/ /usr/local/webapp/nginx/html

EXPOSE 80

LABEL org.opencontainers.image.description="Daily todo application"
