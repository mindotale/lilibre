FROM node:18 AS builder
WORKDIR /app
COPY package.json package-lock.json /app/
RUN npm install
COPY . /app
RUN npm run build --prod

FROM nginx:alpine
COPY --from=builder /app/dist/web /usr/share/nginx/html
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
