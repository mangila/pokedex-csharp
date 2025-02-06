import {useInfiniteQuery} from '@tanstack/react-query';
import {useEffect, useRef} from 'react';
import {PaginationResultDto} from './types';
import {findByPagination} from './api';
import {SessionStorageKeys} from './utils';

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


export const useScrollIntoLastVisitedFragment = () => {
    useEffect(() => {
        const lastFragment = sessionStorage.getItem(SessionStorageKeys.LAST_VISITED_FRAGMENT);
        if (lastFragment) {
            const element = document.getElementById(lastFragment);
            if (element) {
                element.scrollIntoView({behavior: 'instant', block: 'center'});
            }
        }
    }, []);
}
