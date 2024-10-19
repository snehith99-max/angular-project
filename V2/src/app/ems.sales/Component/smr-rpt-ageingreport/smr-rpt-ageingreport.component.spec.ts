import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptAgeingreportComponent } from './smr-rpt-ageingreport.component';

describe('SmrRptAgeingreportComponent', () => {
  let component: SmrRptAgeingreportComponent;
  let fixture: ComponentFixture<SmrRptAgeingreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptAgeingreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptAgeingreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
