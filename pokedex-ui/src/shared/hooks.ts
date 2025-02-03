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

    return {data, loader, isFetchingNextPage, hasNextPage, isLoading, error};
};