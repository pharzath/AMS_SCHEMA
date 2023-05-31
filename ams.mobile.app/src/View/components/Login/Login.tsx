import React from 'react';

import styles from './Login.scss';

export interface LoginProps {
  prop?: string;
}

export function Login({prop = 'default value'}: LoginProps) {
  return <div className={styles.Login}>Login {prop}</div>;
}
