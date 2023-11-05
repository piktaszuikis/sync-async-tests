use may_minihttp::{HttpServer, HttpService, Request, Response};
use std::io;

#[derive(Clone)]
struct HelloWorld;

impl HttpService for HelloWorld {
    fn call(&mut self, _req: Request, rsp: &mut Response) -> io::Result<()> {
        rsp.body("OK");
        Ok(())
    }
}

fn main() {
    let port = 3005;
    let uri = format!("127.0.0.1:{port}");
    println!("Starting webserver {uri}");
    let server = HttpServer(HelloWorld).start(uri).unwrap();
    server.wait();
}

