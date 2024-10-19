import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptProfileandlostreportComponent } from './acc-rpt-profileandlostreport.component';

describe('AccRptProfileandlostreportComponent', () => {
  let component: AccRptProfileandlostreportComponent;
  let fixture: ComponentFixture<AccRptProfileandlostreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptProfileandlostreportComponent]
    });
    fixture = TestBed.createComponent(AccRptProfileandlostreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
