import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private audio: HTMLAudioElement | undefined;

  constructor() { 
   
  }

  playNotificationSound() {
    this.audio = new Audio();
    this.audio.src = 'assets/Audio/notification_ding.mp3';
    this.audio.play();
  }

  loginNotification() {
    this.audio = new Audio();
    this.audio.src = 'assets/Audio/logon_tone.mp3';
    this.audio.play();
  }
}
