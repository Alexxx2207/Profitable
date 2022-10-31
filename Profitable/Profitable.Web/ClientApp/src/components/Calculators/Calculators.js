import { FuturesCalculator } from "./FuturesCalculator/FuturesCalculator";
import styles from "./Calculators.module.css";
import { StocksCalculator } from "./StocksCalculator/StocksCalculator";

export const Calcualtors = () => {
    return (
        <div className={styles.calculatorsContainer}>
            <section className={styles.calculatorContainer}>
                <FuturesCalculator />
            </section>
            <section className={styles.calculatorContainer}>
                <StocksCalculator />
            </section>
        </div>
    );
};
