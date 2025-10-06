# Crystal Arena - Web Client

This is the web client for Crystal Arena, a Final Fantasy Trading Card Game simulation engine. For general project information, see the [main README](../README.md).

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

### Running with Docker (Recommended)

From the project root:
```bash
docker-compose up client
```

### Running Locally

First, install dependencies and run the development server:

```bash
npm install
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

**Note**: The client requires the game engine and image proxy to be running. Use `docker-compose up` from the project root to start all services.

## Development

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses:
- [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font)
- Chakra UI for the component library
- TanStack Query for data fetching and state management

## Project Structure

- `/src/app` - Next.js app router pages and layouts
- `/src/components` - Reusable React components
- `/src/types` - TypeScript type definitions
- `/src/utils` - Utility functions
- `/src/assets` - Static assets

## Contributing

See the [CONTRIBUTING.md](../CONTRIBUTING.md) in the project root for guidelines on contributing to Crystal Arena.

## Learn More About Next.js

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!
