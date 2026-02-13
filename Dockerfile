# dotnet publish must be run before building this image, and the output should be in ./publish/wwwroot/
FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf
COPY entrypoint.sh /entrypoint.sh

RUN apk update \
    && apk add --no-cache jq \
    && apk upgrade \
    && rm -rf /var/cache/apk/* \
    && chmod +x /entrypoint.sh

COPY ./publish/wwwroot/ /usr/local/webapp/nginx/html

EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]

LABEL org.opencontainers.image.description="Daily todo application"
