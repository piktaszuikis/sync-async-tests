const port = 3001;

Bun.serve({
  port: port,
  fetch(req) {
    const url = new URL(req.url);
    if (url.pathname === "/") return new Response("OK");
    return new Response("404!");
  },
});
console.log(`Running on port ${port}`);
