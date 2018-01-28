FROM httpd:latest
ARG source
WORKDIR /app
COPY ${source:-httpd.conf} /usr/local/apache2/conf/
COPY ${source:-dist} /usr/local/apache2/htdocs
