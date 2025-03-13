# Stage 1: Build Angular application
FROM node:18 AS build
WORKDIR /app
COPY . /app
RUN npm install
RUN node_modules/.bin/ng build

# Stage 2: Setup Nginx and serve built Angular application
FROM nginx:alpine
COPY --from=build /app/dist/search-enron /usr/share/nginx/html
EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
