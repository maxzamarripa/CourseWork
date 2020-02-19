import { Observable } from "rxjs";

export function load(url: string) {
    return Observable.create(observer => {
        let xhr = new XMLHttpRequest();

        let onLoad = () => {
            //check response status
            if(xhr.status === 200){
                let data = JSON.parse(xhr.responseText);
                observer.next(data);
                observer.complete();
            } else {
                //Print error text
                observer.error(xhr.statusText);
            }
        };

        xhr.addEventListener("load", onLoad);

        xhr.open("GET", url);
        xhr.send();

        //for unsuscribe
        return () => {
            xhr.removeEventListener("load", onLoad);
            xhr.abort();
        }
    })
    //a process to retry failing request
    .retryWhen(retryStrategy({attempts: 3, delay: 1500}));
};

//Using Fetch
export function loadWithFetch(url:string){
    return Observable.fromPromise(fetch(url)
        .then( r => {
            if (r.status === 200){
                return r.json();
            } else {
                return Promise.reject(r);
            }
        }))
        .retryWhen(retryStrategy());    
}

//For network issues
function retryStrategy({attempts = 4, delay = 1000} = {}){
    return function(errors){
        return errors
            .scan((acc, value) => {
                acc += 1;
                if (acc < attempts){
                    return acc;
                } else {
                    throw new Error(value);
                }
            }, 0)
            .delay(delay);
    }
};