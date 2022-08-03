import { useState, useEffect, useCallback } from 'react';
import { PostsListItem } from './PostsListItem/PostsListItem';
import { loadPostsPage } from '../../../../services/posts/postsService';
import { POSTS_LIST_POSTS_IN_PAGE_COUNT } from '../../../../common/config';
import styles from './PostsList.module.css';

export const PostsList = () => {
    const [posts, setPosts] = useState([]);
    let page = 0;

    const loadPosts = useCallback(() => {
        loadPostsPage(page, POSTS_LIST_POSTS_IN_PAGE_COUNT)
            .then(result => {
                if (result.length > 0) {
                    setPosts(posts => [...posts, ...result])
                }
            });
        page++;
    }, [page]);

    const handleScroll = useCallback((e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

        if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
            loadPosts();
        }
    }, [loadPosts]);

    useEffect(() => {
        loadPosts();
        window.addEventListener("scroll", handleScroll);

        return () => window.removeEventListener('scroll', handleScroll);
    }, [handleScroll, loadPosts]);

    return (
        <div className={styles.wrapper}>
            <h1 className={styles.discoverHeading}>Discover</h1>
            <div className={styles.postsList}>
                {posts.map(post => <PostsListItem key={post.guid} {...post} />)}
            </div>
        </div>
    );
}