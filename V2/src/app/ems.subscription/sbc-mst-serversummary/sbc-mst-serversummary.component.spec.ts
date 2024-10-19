import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstServersummaryComponent } from './sbc-mst-serversummary.component';

describe('SbcMstServersummaryComponent', () => {
  let component: SbcMstServersummaryComponent;
  let fixture: ComponentFixture<SbcMstServersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstServersummaryComponent]
    });
    fixture = TestBed.createComponent(SbcMstServersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
