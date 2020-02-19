import { Observable } from "rxjs";
import { load, loadWithFetch } from "./loader";

//Circle that follows the mouse 
let circle = document.getElementById("circle");
let sourceCircle = Observable.fromEvent(document, "mousemove")
    .map((e: MouseEvent) => {
        return {
            x: e.clientX,
            y: e.clientY
        }
    })
    .filter(value => value.x < 500)
    .delay(300);

function onNextMovment(value) {
    circle.style.left = value.x;
    circle.style.top = value.y;
};

sourceCircle.subscribe(
    onNextMovment,
    e => console.log(`error: ${e}`),
    () => console.log("complete")
);

//Movies button
let output = document.getElementById("output");
let button = document.getElementById("button");
let click = Observable.fromEvent(button, "click");

function renderMovies (movies){
    movies.forEach(movie => {
        let div = document.createElement("div");
        div.innerText = movie.title;
        output.appendChild(div);
    });
};

click.flatMap(e => loadWithFetch("movies.json"))
.subscribe(
    renderMovies,
    e => console.log(`error: ${e}`),
    () => console.log("complete")
);