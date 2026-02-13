# dotnet publish must be run before building this image, and the output should be in ./publish/wwwroot/
FROM nginx:alpine

RUN apk update \
    && apk add --no-cache jq \
    && apk upgrade \
    && rm -rf /var/cache/apk/*

COPY nginx.conf /etc/nginx/nginx.conf

COPY ./publish/wwwroot/ /usr/local/webapp/nginx/html

COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]

LABEL org.opencontainers.image.description="Daily todo application"
