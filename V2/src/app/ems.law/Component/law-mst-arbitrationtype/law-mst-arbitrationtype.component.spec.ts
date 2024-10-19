import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstArbitrationtypeComponent } from './law-mst-arbitrationtype.component';

describe('LawMstArbitrationtypeComponent', () => {
  let component: LawMstArbitrationtypeComponent;
  let fixture: ComponentFixture<LawMstArbitrationtypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstArbitrationtypeComponent]
    });
    fixture = TestBed.createComponent(LawMstArbitrationtypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
