#include <sys/socket.h>\
#include <netinet/in.h>\
#include <stdio.h>\
#include <string.h>\
\
int main(int argc, char **argv)\{\
  int sockfd = socket(AF_INET,SOCK_STREAM,0);\
  char line[5000];\
  char line2[5000];\
  struct sockaddr_in serveraddr;\
  \
  serveraddr.sin_family=AF_INET;\
  serveraddr.sin_port=htons(9875);\
  serveraddr.sin_addr.s_addr=inet_addr("127.0.0.1");\
\
  int e = connect(sockfd,(struct sockaddr*)&serveraddr,\
           sizeof(struct sockaddr_in));\
  if(e<0)\{\
    printf("Some problems with connecting\\n");\
    return -1;\
  \}\
\
  printf("Enter a line: ");\
  fgets(line,5000,stdin);\
\
  send(sockfd,line,strlen(line),0);\
  recv(sockfd,line2,5000,0);\
  printf("Got from server: %s\\n",line2);\
\
\
  return 0;\
\}\
\
\
\
}