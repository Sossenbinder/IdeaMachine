# Ideamachine

Ideamachine is meant to be a tool to share project ideas for developers, which we often lack due to creativity blackouts. Everyone can submit an Idea, and also connect with / be connected with interested people to talk about an idea.

## Getting Started

Just clone & run. It's 2021, we don't do static installations on a machine anymore ;)

## Tech stack

* Asp.Net 5
* React + Redux + React-Query + TS + Less + Webpack = <3
* MassTransit over RabbitMQ
* grpc with https://github.com/protobuf-net/protobuf-net.Grpc
* MSSql
* Serilog + Seq

Still to come:
* Kubernetes .yaml files
* Monitoring (Prometheus & Grafana)

### Dependencies

* Docker

### Installing

* Just download and run. For now, there is no productive deployment yet, and no shared public database. The compose fill will create a persistent postgresql volume for data.
