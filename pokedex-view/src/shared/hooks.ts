import {useEffect, useRef} from "react";
import {LAST_VISITED_FRAGMENT} from "./utils";
import {useInfiniteQuery} from "@tanstack/react-query";
import {PaginationResultDto} from "./types";
import {findByPagination} from "./api";

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

export const useInfiniteScroll = (
    queryKey: readonly unknown[],
    pageSize: number,
    typesFilter: string[],
    specialFilter: string[]) => {
    const loader = useRef(null);
    const {
        data,
        hasNextPage,
        fetchNextPage,
        isLoading,
        error
    } = useInfiniteQuery<PaginationResultDto, Error>({
        queryKey: queryKey,
        initialPageParam: 1,
        refetchOnMount: true,
        queryFn: ({pageParam}) =>
            findByPagination(pageParam as number, pageSize, typesFilter, specialFilter),
        getNextPageParam: (lastPage: PaginationResultDto) => {
            return lastPage.current_page + 1;
        }
    });

    useEffect(() => {
        const currentLoader = loader.current;
        const observer = new IntersectionObserver(
            (entries) => {
                if (entries[0].isIntersecting && hasNextPage) {
                    fetchNextPage();
                }
            },
            {threshold: 1.0}
        );

        if (currentLoader) {
            observer.observe(currentLoader);
        }

        return () => {
            if (currentLoader) {
                observer.unobserve(currentLoader);
            }
        };
    }, [hasNextPage, fetchNextPage]);

    return {data, loader, isLoading, error};
};