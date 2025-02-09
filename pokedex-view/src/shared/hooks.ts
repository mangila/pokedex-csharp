import {useEffect} from "react";
import {LAST_VISITED_FRAGMENT} from "./utils";

export const useScrollIntoLastVisitedFragment = () => {
    useEffect(() => {
        const lastFragment = sessionStorage.getItem(LAST_VISITED_FRAGMENT);
        if (lastFragment) {
            const element = document.getElementById(lastFragment);
            if (element) {
                element.scrollIntoView({behavior: 'instant', block: 'start'});
            }
        }
    }, []);
}