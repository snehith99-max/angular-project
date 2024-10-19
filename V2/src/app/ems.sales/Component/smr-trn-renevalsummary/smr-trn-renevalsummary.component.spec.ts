import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenevalsummaryComponent } from './smr-trn-renevalsummary.component';

describe('SmrTrnRenevalsummaryComponent', () => {
  let component: SmrTrnRenevalsummaryComponent;
  let fixture: ComponentFixture<SmrTrnRenevalsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenevalsummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenevalsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
