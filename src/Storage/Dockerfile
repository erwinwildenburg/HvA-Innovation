FROM node:latest
ARG source

# Copy init_container.sh to the /bin directory
COPY ${source:-init_container.sh} /bin/
RUN chmod 755 /bin/init_container.sh

# Configure ports
EXPOSE 8081

# Configure the node.js app
COPY . /app

WORKDIR /app

ENTRYPOINT ["/bin/init_container.sh"]
