import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrBobateaDashboardComponent } from './smr-bobatea-dashboard.component';

describe('SmrBobateaDashboardComponent', () => {
  let component: SmrBobateaDashboardComponent;
  let fixture: ComponentFixture<SmrBobateaDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrBobateaDashboardComponent]
    });
    fixture = TestBed.createComponent(SmrBobateaDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
