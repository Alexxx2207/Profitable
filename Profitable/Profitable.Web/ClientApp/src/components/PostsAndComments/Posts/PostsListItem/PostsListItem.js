import styles from './PostsListItem.module.css';

export const PostsListItem = (props) => {
    return (<div className={styles.post}>
        <div className={styles.content}>
            <h2>{props.title}</h2>
        </div>
        <div className={styles.information}>
            <div className={styles.author}>
                {props.author}
            </div>
            <div className={styles.postedOn}>
                {props.postedOn}
            </div>
        </div>
    </div>);
}