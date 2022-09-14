import { FuturesCalculator } from './FuturesCalculator/FuturesCalculator';
import styles from './Calculators.module.css';

export const Calcualtors = () => {
    return (
        <div className={styles.calculatorsContainer}>
            <section className={styles.futuresCalculatorContainer}>
                <FuturesCalculator />
            </section>
        </div>
    );
}