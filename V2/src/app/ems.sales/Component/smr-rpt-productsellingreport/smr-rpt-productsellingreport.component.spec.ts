import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptProductsellingreportComponent } from './smr-rpt-productsellingreport.component';

describe('SmrRptProductsellingreportComponent', () => {
  let component: SmrRptProductsellingreportComponent;
  let fixture: ComponentFixture<SmrRptProductsellingreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptProductsellingreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptProductsellingreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
