import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private _alert: IAlert = {
    message: '',
    type: 'success',
    isVisible: false,
  };

  private _alertSubject = new BehaviorSubject<IAlert>(this._alert);

  constructor() {}

  show(alert: string, alertType: 'success' | 'info' | 'warning' | 'error') {
    console.log(alert)
    if (this._alertSubject.getValue().isVisible) {
      this.hide();
      setTimeout(() => {
        this._alertSubject.next({
          message: alert,
          type: alertType,
          isVisible: true,
        });
      }, 400);
    } else {
      this._alertSubject.next({
        message: alert,
        type: alertType,
        isVisible: true,
      });
    }
  }

  hide() {
    this._alertSubject.next({ message: '', type: 'success', isVisible: false });
  }

  getAlert(): Observable<IAlert> {
    return this._alertSubject.asObservable();
  }
}

export interface IAlert {
  message: string;
  type: 'success' | 'info' | 'warning' | 'error';
  isVisible: boolean;
}
