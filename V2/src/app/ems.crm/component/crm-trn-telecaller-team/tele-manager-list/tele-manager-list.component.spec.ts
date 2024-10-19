import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeleManagerListComponent } from './tele-manager-list.component';

describe('TeleManagerListComponent', () => {
  let component: TeleManagerListComponent;
  let fixture: ComponentFixture<TeleManagerListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TeleManagerListComponent]
    });
    fixture = TestBed.createComponent(TeleManagerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
