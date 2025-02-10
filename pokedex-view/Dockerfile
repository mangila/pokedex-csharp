FROM node:22-alpine AS builder
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build

# Install a simple static file server to serve the built files
RUN npm install -g serve

# Expose the port that the app will run on
EXPOSE 4020

# Start the app using the static file server
CMD ["serve", "-s", "dist", "-l", "4020"]