import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstSequencecustomizerComponent } from './smr-mst-sequencecustomizer.component';

describe('SmrMstSequencecustomizerComponent', () => {
  let component: SmrMstSequencecustomizerComponent;
  let fixture: ComponentFixture<SmrMstSequencecustomizerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstSequencecustomizerComponent]
    });
    fixture = TestBed.createComponent(SmrMstSequencecustomizerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
