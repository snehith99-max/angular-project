import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstConsumersummaryComponent } from './sbc-mst-consumersummary.component';

describe('SbcMstConsumersummaryComponent', () => {
  let component: SbcMstConsumersummaryComponent;
  let fixture: ComponentFixture<SbcMstConsumersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstConsumersummaryComponent]
    });
    fixture = TestBed.createComponent(SbcMstConsumersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
