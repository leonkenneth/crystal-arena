FROM node:20

WORKDIR /app

# Install build dependencies for canvas
RUN apt-get update && \
    apt-get install -y \
    python3 \
    build-essential \
    libcairo2-dev \
    libpango1.0-dev \
    libjpeg-dev \
    libgif-dev \
    librsvg2-dev \
    && rm -rf /var/lib/apt/lists/*

# Copy source code
COPY . .

# Install dependencies
RUN rm -rf node_modules && npm install

# Expose port
EXPOSE 4000

# Start server
CMD ["npm", "run", "dev"]
