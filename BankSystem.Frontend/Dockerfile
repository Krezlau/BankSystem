FROM node:20 AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build --prod

FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY ./ssl/localhost.crt /etc/nginx/ssl/localhost.crt
COPY ./ssl/localhost.key /etc/nginx/ssl/localhost.key
COPY --from=build /app/dist/bank-system.frontend/browser /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]

