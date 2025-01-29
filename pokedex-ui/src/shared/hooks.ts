import {useInfiniteQuery} from '@tanstack/react-query';
import {useEffect, useRef} from 'react';
import {PaginationResultDto} from './types';
import {getAllPokemons} from './api';

export const useInfiniteScroll = (queryKey: readonly unknown[], pageSize: number) => {
    const loader = useRef(null);
    const {
        data,
        isFetchingNextPage,
        hasNextPage,
        fetchNextPage,
        isLoading,
        error
    } = useInfiniteQuery<PaginationResultDto, Error>({
        queryKey: queryKey,
        initialPageParam: 1,
        refetchOnMount: true,
        retry: 3,
        queryFn: ({pageParam}) =>
            getAllPokemons(pageParam as number, pageSize),
        getNextPageParam: (lastPage: PaginationResultDto) => {
            return lastPage.current_page + 1;
        }
    });

    useEffect(() => {
        const observer = new IntersectionObserver(
            (entries) => {
                if (entries[0].isIntersecting && hasNextPage) {
                    fetchNextPage();
                }
            },
            {threshold: 1.0}
        );

        if (loader.current) {
            observer.observe(loader.current);
        }

        return () => {
            if (loader.current) {
                observer.unobserve(loader.current);
            }
        };
    }, [hasNextPage, fetchNextPage]);

    return {data , loader, isFetchingNextPage, isLoading, error};
};