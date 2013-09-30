#include <sys/socket.h>\
#include <netinet/in.h>\
#include <stdio.h>\
#include <string.h>\
\
int main(int argc, char **argv)\{\
\
  int sockfd = socket(AF_INET,SOCK_STREAM,0);\
  char line[5000];\
  int len;\
\
  struct sockaddr_in serveraddr, clientaddr;\
  struct timeval to;\
  \
  serveraddr.sin_family=AF_INET;\
  serveraddr.sin_port=htons(9875);\
  serveraddr.sin_addr.s_addr=htonl(INADDR_ANY);\
\
  to.tv_sec=5;\
  to.tv_usec=0;\
\
  bind(sockfd,(struct sockaddr*)&serveraddr,sizeof(serveraddr));\
  listen(sockfd,10);\
\
  //setsockopt(sockfd,SOL_SOCKET,SO_RCVTIMEO,&to,sizeof(to));\
\
  len=sizeof(clientaddr);\
  while(1)\{\
    int clientsocket = accept(sockfd,\
                              (struct sockaddr*)&clientaddr,&len);\
    int pid=fork();\
    if(pid==0)\{\
      int n;\
      n=recv(clientsocket,line,5000,0);\
      if(n<0)\{\
          printf("Sorry, the client was too slow\\n");\
          return 1;\
      \}\
      printf("Got %d bytes from client: %s\\n",n,line);\
      send(clientsocket,line,strlen(line),0);\
      close(clientsocket);\
      return 0;\
    \}\
  \}\
\
  return 0;\
\}\
}