# Build stage
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files
COPY idv-demo-ui/package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY idv-demo-ui/ ./

# Build the app for production
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy built app to nginx
COPY --from=build /app/build /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Create nginx user and set permissions
RUN chown -R nginx:nginx /usr/share/nginx/html && \
    chown -R nginx:nginx /var/cache/nginx && \
    chown -R nginx:nginx /var/log/nginx && \
    chown -R nginx:nginx /etc/nginx/conf.d && \
    touch /var/run/nginx.pid && \
    chown -R nginx:nginx /var/run/nginx.pid

# Switch to non-root user
USER nginx

EXPOSE 10000

CMD ["nginx", "-g", "daemon off;"]