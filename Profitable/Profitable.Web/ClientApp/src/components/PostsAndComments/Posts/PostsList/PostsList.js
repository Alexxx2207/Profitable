import { PostsListItem } from './PostsListItem/PostsListItem';

import styles from './PostsList.module.css';

export const PostsList = ({posts}) => {
    return (
        <div className={styles.postsList}>
            {posts.map(post => <PostsListItem key={post.guid} post={post} />)}
        </div>
    );
}