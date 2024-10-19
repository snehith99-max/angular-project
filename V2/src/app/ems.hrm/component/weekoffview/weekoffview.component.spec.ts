import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeekoffviewComponent } from './weekoffview.component';

describe('WeekoffviewComponent', () => {
  let component: WeekoffviewComponent;
  let fixture: ComponentFixture<WeekoffviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WeekoffviewComponent]
    });
    fixture = TestBed.createComponent(WeekoffviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
