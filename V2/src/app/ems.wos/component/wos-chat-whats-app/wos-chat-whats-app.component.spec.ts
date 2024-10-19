import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WosChatWhatsAppComponent } from './wos-chat-whats-app.component';

describe('WosChatWhatsAppComponent', () => {
  let component: WosChatWhatsAppComponent;
  let fixture: ComponentFixture<WosChatWhatsAppComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WosChatWhatsAppComponent]
    });
    fixture = TestBed.createComponent(WosChatWhatsAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
