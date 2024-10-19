import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstSequencecodeeditComponent } from './smr-mst-sequencecodeedit.component';

describe('SmrMstSequencecodeeditComponent', () => {
  let component: SmrMstSequencecodeeditComponent;
  let fixture: ComponentFixture<SmrMstSequencecodeeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstSequencecodeeditComponent]
    });
    fixture = TestBed.createComponent(SmrMstSequencecodeeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
