

export const changeStateValuesForControlledForms = (values, inputTargetToChange) =>{
    return {
        ...values,
        [inputTargetToChange.name]: inputTargetToChange.value
    }
}