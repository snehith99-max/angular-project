import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstMaptaxsegment2productComponent } from './smr-mst-maptaxsegment2product.component';

describe('SmrMstMaptaxsegment2productComponent', () => {
  let component: SmrMstMaptaxsegment2productComponent;
  let fixture: ComponentFixture<SmrMstMaptaxsegment2productComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstMaptaxsegment2productComponent]
    });
    fixture = TestBed.createComponent(SmrMstMaptaxsegment2productComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
