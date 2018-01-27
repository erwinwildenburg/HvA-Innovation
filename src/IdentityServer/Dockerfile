FROM microsoft/aspnetcore:latest
ARG source

# Run apt-get, to install the SSH server
RUN apt-get update \
  && apt install -y --no-install-recommends openssh-server \
  && echo "root:Docker!" | chpasswd \
  && echo "cd /home" >> /etc/bash.bashrc

# Copy the sshd_config file to its new location
COPY ${source:-sshd_config} /etc/ssh/

# Copy init_container.sh to the /bin directory
COPY ${source:-init_container.sh} /bin/
RUN chmod 755 /bin/init_container.sh

# Create required directories
RUN mkdir -p /home/LogFiles \
	&& mkdir -p /defaulthome/hostingstart

# Configure ports
EXPOSE 8080 2222

# Configure the dotnet core app
ENV PORT 8080
ENV WEBSITE_ROLE_INSTANCE_ID localRoleInstance
ENV WEBSITE_INSTANCE_ID localInstance
ENV ASPNETCORE_URLS=
ENV PATH ${PATH}:/home/site/wwwroot
COPY ${source:-obj/Docker/publish} /defaulthome/hostingstart

WORKDIR /home/site/wwwroot

ENTRYPOINT ["/bin/init_container.sh"]
